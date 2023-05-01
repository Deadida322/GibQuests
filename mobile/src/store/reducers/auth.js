import { LOGIN_USER} from "../types"

const initialState = {
    user: {},
    isLoggedIn: false
}

export const authReducer = (state = initialState, action)=>{
    console.log(action)

    switch (action.type){
        
        case LOGIN_USER:{

            return {
                ...state, 
                user: action.payload,
                isLoggedIn: true
            }
        }
        default :{
            return state
        }
        
    }
}