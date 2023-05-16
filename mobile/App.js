import { useState } from "react";
import { StatusBar } from "react-native";
import "react-native-gesture-handler";
import AppLoading from "expo-app-loading";
import { bootstrap } from "./src/bootstrap";
import AppNavigation from "./src/navigation/AppNavigation";
import { THEME } from "./src/theme";
import { AppRegistry } from "react-native";
import { Provider as PaperProvider } from "react-native-paper";

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
            <StatusBar
                backgroundColor={THEME.PRIMARY_COLOR}
                barStyle="default"
            ></StatusBar>
            <AppNavigation />
        </PaperProvider>
    );
}
AppRegistry.registerComponent("main", () => Main);
