import { useState } from "react";
import { Text, View, StatusBar } from "react-native";
import "react-native-gesture-handler";
import AppLoading from "expo-app-loading";
import { bootstrap } from "./src/bootstrap";
import AppNavigation from "./src/navigation/AppNavigation";
import { Provider } from "react-redux";
import store from "./src/store";
import { THEME } from "./src/theme";
import { AppRegistry } from 'react-native';
import {
    Provider as PaperProvider,
} from "react-native-paper";

export default function App() {
    const [ready, setIsReady] = useState(false);

    if (!ready)
        return (
            <AppLoading
                startAsync={() => bootstrap()}
                onFinish={() => setIsReady(true)}
                onError={(err) => console.log(err)}
            />
        );

    return (
        <PaperProvider>
            <Provider store={store}>
                <StatusBar
                    backgroundColor={THEME.PRIMARY_COLOR}
                    barStyle="default"
                ></StatusBar>
                <AppNavigation />
            </Provider>
        </PaperProvider>
    );
}
AppRegistry.registerComponent("main", () => Main);