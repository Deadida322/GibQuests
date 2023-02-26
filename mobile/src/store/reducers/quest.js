import { SET_CURRENT_EDIT} from "../types"

const initialState = {
    currenEditableQuest: {},
}

export const questReducer = (state = initialState, action)=>{
    switch (action.type){
        case SET_CURRENT_EDIT:{
            return {
                ...state, currenEditableQuest: action.payload
            }
        }
        default :{
            return state
        }
        
    }
}