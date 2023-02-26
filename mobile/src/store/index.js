import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from 'redux-thunk'
import { postReducer } from "./reducers/post";
import { questReducer } from "./reducers/quest";

const rootReducer = combineReducers({post: postReducer, quest: questReducer})

export default store = createStore(rootReducer, applyMiddleware(thunk))