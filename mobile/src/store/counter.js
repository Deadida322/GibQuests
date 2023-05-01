import { makeAutoObservable, action, observable, runInAction } from "mobx";
import AsyncStorage from "@react-native-async-storage/async-storage";
import request from "../request";

export function counter() {
    return makeAutoObservable(
        {
            counter: false,
            increment() {
                runInAction(() => {
                    this.counter++;
                    console.log(this.counter)
                });
            },
            
        },
        {
            counter: observable,
            increment: action.bound,
        }
    );
}
export default counter();
