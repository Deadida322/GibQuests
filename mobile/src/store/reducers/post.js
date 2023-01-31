import { CREATE_POST, LOAD_POSTS, REMOVE_POST, TOGGLE_BOOKED } from "../types"

const initialState = {
    allPosts: [],
    bookedPosts: [],
    loading: true
}

export const postReducer = (state = initialState, action)=>{
    switch (action.type){
        case LOAD_POSTS:{
            return {
                ...state, allPosts: action.payload, bookedPosts: action.payload.filter(item=>item.booked), loading: false
            }
        }
        case TOGGLE_BOOKED:{
            const allPosts = state.allPosts.map(item=>{
                if (item.id === action.payload){
                    item.booked = !item.booked
                }
                return item
            })
            return {
                ...state, allPosts, bookedPosts: allPosts.filter(item=>item.booked)
            }
        }
        case REMOVE_POST:{
            return {...state, 
                allPosts: state.allPosts.filter(item=>item.id != action.payload), 
                bookedPosts: state.bookedPosts.filter(item=>item.id != action.payload)
            }
        }
        case CREATE_POST:{
            return {
                ...state,
                allPosts: [action.payload, ...state.allPosts]
            }
        }
        default: {
            return state
        }
    }
}