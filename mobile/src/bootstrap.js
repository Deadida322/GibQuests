import * as Font from "expo-font";
import { DB } from "./db";

export async function bootstrap() {
    try {
        await Font.loadAsync({
            regular: require("../assets/fonts/Manrope-Regular.ttf"),
            light: require("../assets/fonts/Manrope-Light.ttf"),
            bold: require("../assets/fonts/Manrope-Bold.ttf"),
            ionicons: require("@expo/vector-icons/build/vendor/react-native-vector-icons/Fonts/Ionicons.ttf"),
            anticon: require("@expo/vector-icons/build/vendor/react-native-vector-icons/Fonts/AntDesign.ttf"),
            "material-community": require("@expo/vector-icons/build/vendor/react-native-vector-icons/Fonts/MaterialCommunityIcons.ttf"),
        });
        await DB.init();
    } catch (e) {
        console.log(e);
    }
}
