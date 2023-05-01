import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from 'redux-thunk'
import { postReducer } from "./reducers/post";
import { questReducer } from "./reducers/quest";
import { authReducer } from "./reducers/auth";

const rootReducer = combineReducers({post: postReducer, quest: questReducer, auth: authReducer})

export default store = createStore(rootReducer, applyMiddleware(thunk))