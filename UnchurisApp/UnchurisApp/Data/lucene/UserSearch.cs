using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using UnchurisApp.Models;
using Version = Lucene.Net.Util.Version;

namespace UnchurisApp.Data.lucene {
  public class UserSearch {
    public static string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene__index_user");
    private static FSDirectory _directoryTemp;
    public static FSDirectory _directory {
      get {
        if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
        if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
        var lockFilePath = Path.Combine(_luceneDir, "write.lock");
        if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
        return _directoryTemp;
      }
    }
    private static List<User> _mapLuceneToDataList(IEnumerable<Document> hits) {
      return hits.Select(_mapLuceneDocumentToData).ToList();
    }
    private static List<User> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
        IndexSearcher searcher) {
      return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
    }

    private static void _addToLuceneIndex(User userModel, IndexWriter writer) {
      // remove older index entry
      var searchQuery = new TermQuery(new Term("Id", userModel.Id.ToString()));
      writer.DeleteDocuments(searchQuery);

      // add new index entry
      var doc = new Document();
      // add lucene fields mapped to db fields
      doc.Add(new Field("Id", userModel.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("Username", userModel.Username, Field.Store.YES, Field.Index.ANALYZED));
      doc.Add(new Field("ProfileName", userModel.Profile.Name, Field.Store.YES, Field.Index.ANALYZED));
      doc.Add(new Field("DateCreated", userModel.DateCreated.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("AdvertisementsCount", userModel.Advertisements.Count.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("ProfileBio", userModel.Profile.Bio, Field.Store.YES, Field.Index.ANALYZED));
      // add entry to index
      writer.AddDocument(doc);
    }

    public static void AddUpdateLuceneIndex(IEnumerable<User> sampleDatas) {
      // init lucene
      var analyzer = new StandardAnalyzer(Version.LUCENE_30);
      using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)) {
        // add data to lucene search index (replaces older entry if any)
        foreach (var sampleData in sampleDatas) _addToLuceneIndex(sampleData, writer);

        // close handles
        analyzer.Close();
        writer.Dispose();
      }
    }

    public static void AddUpdateLuceneIndex(User sampleData) {
      AddUpdateLuceneIndex(new List<User> { sampleData });
    }

    public static void ClearLuceneIndexRecord(int record_id) {
      // init lucene
      var analyzer = new StandardAnalyzer(Version.LUCENE_30);
      using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)) {
        // remove older index entry
        var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
        writer.DeleteDocuments(searchQuery);

        // close handles
        analyzer.Close();
        writer.Dispose();
      }
    }

    public static bool ClearLuceneIndex() {
      try {
        var analyzer = new StandardAnalyzer(Version.LUCENE_30);
        using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED)) {
          // remove older index entries
          writer.DeleteAll();

          // close handles
          analyzer.Close();
          writer.Dispose();
        }
      } catch (Exception) {
        return false;
      }
      return true;
    }

    public static void Optimize() {
      var analyzer = new StandardAnalyzer(Version.LUCENE_30);
      using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED)) {
        analyzer.Close();
        writer.Optimize();
        writer.Dispose();
      }
    }

    private static User _mapLuceneDocumentToData(Document doc) {
      var profile = new UserProfile {
        Name = doc.Get("ProfileName"),
        Bio = doc.Get("ProfileBio")
      };
      var user = new User {
        Profile = profile,
        Id = Convert.ToInt32(doc.Get("Id")),
        Username = doc.Get("Username"),
        DateCreated = Convert.ToDateTime(doc.Get("DateCreated")),
      };
      return user;
    }

    private static Query ParseQuery(string searchQuery, QueryParser parser) {
      Query query;
      try {
        query = parser.Parse(searchQuery.Trim());
      } catch (ParseException) {
        query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
      }
      return query;
    }

    private static FuzzyQuery FparseQuery(string filed, string searchQuery) {
      FuzzyQuery query = new FuzzyQuery(new Term(filed, searchQuery));
      return query;
    }

    private static List<User> _search(string searchQuery, string searchField = "") {
      // validation
      if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) {
        return new List<User>();
      }

      // set up lucene searcher
      using (var searcher = new IndexSearcher(_directory, false)) {
        var hits_limit = 1000;
        var analyzer = new StandardAnalyzer(Version.LUCENE_30);

        // search by single field
        if (!string.IsNullOrEmpty(searchField)) {
          var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
          var query = ParseQuery(searchQuery, parser);
          query = FparseQuery(searchField, searchQuery);
          var hits = searcher.Search(query, hits_limit).ScoreDocs;
          var results = _mapLuceneToDataList(hits, searcher);
          analyzer.Close();
          searcher.Dispose();
          return results;
        }
        // search by multiple fields (ordered by RELEVANCE)
        else {
          var parser = new MultiFieldQueryParser
              (Version.LUCENE_30, new[] { "Id", "Username", "ProfileName", "DateCreated", "ProfileBio" }, analyzer);
          var query = ParseQuery(searchQuery, parser);
          query = FparseQuery(searchField, searchQuery);
          var hits = searcher.Search
          (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
          var results = _mapLuceneToDataList(hits, searcher);
          analyzer.Close();
          searcher.Dispose();
          return results;
        }
      }
    }

    public static List<User> Search(string input, string fieldName = "") {
      if (string.IsNullOrEmpty(input)) {
        return new List<User>();
      }

      var terms = input.Trim().Replace("-", " ").Split(' ')
          .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
      input = string.Join(" ", terms);

      return _search(input, fieldName);
    }

    public static List<User> GetByIndexRecords(string searchQuery) {
      string searchField = "Id";

      // set up lucene searcher
      using (var searcher = new IndexSearcher(_directory, false)) {
        var hits_limit = 1000;
        var analyzer = new StandardAnalyzer(Version.LUCENE_30);

        // search by single field
        var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
        var query = ParseQuery(searchQuery, parser);
        var hits = searcher.Search(query, hits_limit).ScoreDocs;
        var results = _mapLuceneToDataList(hits, searcher);
        analyzer.Close();
        searcher.Dispose();
        return results;
      }
    }

    public static List<User> GetAllIndexRecords() {
      // validate search index
      if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) {
        return new List<User>();
      }

      // set up lucene searcher
      var searcher = new IndexSearcher(_directory, false);
      var reader = IndexReader.Open(_directory, false);
      var docs = new List<Document>();
      var term = reader.TermDocs();
      while (term.Next()) docs.Add(searcher.Doc(term.Doc));
      reader.Dispose();
      searcher.Dispose();
      return _mapLuceneToDataList(docs);
    }
  }
}