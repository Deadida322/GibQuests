import { SET_CURRENT_EDIT} from "../types"

export const setCurrentQuest = quest =>{
    return async dispatch =>{
        dispatch({
            type: SET_CURRENT_EDIT,
            payload: quest
        })
    }
}