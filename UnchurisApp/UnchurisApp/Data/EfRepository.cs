using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UnchurisApp.Data {

  public class EfRepository<T> : IRepository<T>
        where T : class {
    protected DbContext Context;
    protected readonly bool ShareContext;

    public EfRepository(DbContext context) : this(context, false) { }
    public EfRepository(DbContext context, bool sharedContext) {
      Context = context;
      ShareContext = sharedContext;
    }

    protected DbSet<T> DbSet {
      get {
        return Context.Set<T>();
      }
    }

    public IQueryable<T> All() {
      return DbSet.AsQueryable();
    }

    public bool Any(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {
      return DbSet.Any(predicate);
    }

    public int Count {
      get { return DbSet.Count(); }
    }

    public T Create(T t) {
      var rez = DbSet.Add(t);

      if (!ShareContext) {
        Context.SaveChanges();
      }

      return rez;
    }

    public T Delete(T t) {
      var rez = DbSet.Remove(t);

      if (!ShareContext) {
        Context.SaveChanges();
      }

      return rez;
    }

    public int Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {
      var records = FindAll(predicate);

      foreach (var record in records) {
        DbSet.Remove(record);
      }

      if (!ShareContext) {
        return Context.SaveChanges();
      }

      return 0;
    }

    public T Find(params object[] keys) {
      return DbSet.Find(keys);
    }

    public T Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {
      return DbSet.SingleOrDefault(predicate);
    }

    public IQueryable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {
      return DbSet.Where(predicate).AsQueryable();
    }

    public IQueryable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int index, int size) {
      var skip = index * size;

      IQueryable<T> query = DbSet;

      if (predicate != null) {
        query = query.Where(predicate);
      }

      if (skip != 0) {
        query = query.Skip(skip);
      }

      return query.Take(size).AsQueryable();

    }

    public T Update(T t) {
      var entry = Context.Entry(t);

      var rez = DbSet.Attach(t);

      entry.State = EntityState.Modified;

      if (!ShareContext) {
        Context.SaveChanges();
      }

      return rez;
    }

    public void Dispose() {
      if (!ShareContext && Context != null) {
        try {
          Context.Dispose();
        } catch { }
      }
    }
  }
}