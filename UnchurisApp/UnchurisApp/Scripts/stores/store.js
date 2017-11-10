import { createStore, applyMiddleware } from 'redux';
import { loadTranslations, setLocale, syncTranslationWithStore } from 'react-redux-i18n';
import thunk from 'redux-thunk';

import rootReducer from '../reducers/root-reducer';
import translationsObject from '../localization/translationsObject';

const store = createStore(
    rootReducer,
    applyMiddleware(thunk)
);
syncTranslationWithStore(store);
store.dispatch(loadTranslations(translationsObject));
store.dispatch(setLocale('ru'));

export default store;