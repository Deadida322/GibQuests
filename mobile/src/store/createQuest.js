import { makeAutoObservable, action, observable, runInAction } from "mobx";
import AsyncStorage from "@react-native-async-storage/async-storage";
import request from "../request";

export function questStore() {
    return makeAutoObservable(
        {
            currentQuest: {
                stages: []
            },
            setCurrentEditable(data) {
                this.currentQuest = data;
            },
            clearCurrentEditable(){
                this.currentQuest = {stages:[]}
            },
            get(){
                request({
                    url: `/GenerateQuest/GetQuest/${this.currentQuest.id}`,
                    methods: "GET",
                    
                }).then((res)=>{
                    this.setCurrentEditable(res)
                })
            }
        },
        {
            currentQuest: observable,
            setCurrentEditable: action.bound,
            clearCurrentEditable: action.bound,
        }
    );
}
const store = questStore()
export default store;
export const currentQuest = questStore().currentQuest
export const setCurrentEditable = questStore().setCurrentEditable
