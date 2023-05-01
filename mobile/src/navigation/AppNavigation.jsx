import { NavigationContainer } from '@react-navigation/native';
import { View, Image, TouchableNativeFeedback } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { HeaderButtons, Item } from 'react-navigation-header-buttons'
import AppHeaderIcon from '../components/AppHeaderIcon';
import MainScreen from '../screens/MainScreen';
import PostScreen from '../screens/PostScreen';
import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { THEME } from '../theme';
import BookMarkedScreen from '../screens/BookMarkedScreen'
import { Ionicons } from '@expo/vector-icons';
import { createDrawerNavigator, DrawerItemList } from '@react-navigation/drawer';
import AboutScreen from '../screens/AboutScreen'
import { Text, Avatar } from 'react-native-paper';
import CreateScreen from '../screens/CreateScreen'
import AddText from '../screens/Edit/AddText';
import AddVideo from "../screens/Edit/AddVideo"
import AddQR from "../screens/Edit/AddQR"
import AddMap from '../screens/Edit/AddMap';
import AddTestMain from '../screens/AddTest/AddTestMain';
import AddSingleQuestion from '../screens/AddTest/AddSingleQuestion';
import AddMutlipleQuestion from '../screens/AddTest/AddMultipleQuestion'
import AddInsertQuestion from '../screens/AddTest/AddInsertQuestion'
import AddOrderQuestion from '../screens/AddTest/AddOrderQuestion'
import PreviewScreen from '../screens/GoQuest/PreviewScreen';
import GoScreen from '../screens/GoQuest/GoScreen';
import LoginScreen from '../screens/Auth/LoginScreen';
import RegisterScreen from '../screens/Auth/RegisterScreen';
import WatchScreen from '../screens/Watch';

const Drawer = createDrawerNavigator();
const Stack = createNativeStackNavigator();
const Add = createNativeStackNavigator();
const Tab = createMaterialBottomTabNavigator()
const Create = createNativeStackNavigator();
const About = createNativeStackNavigator();
const AddTest = createNativeStackNavigator();
const InitialStack = createNativeStackNavigator();


const stackConfig = {
    animation: "slide_from_left",
    animationDuration: "100",
    headerStyle: {
        backgroundColor: THEME.colors.primary,
    },
    headerTintColor: 'white',
    headerTitleStyle: {
        fontFamily: 'regular'
    },
}


export const AddTestNavigator = ({ navigation, route }) => {
    return (<AddTest.Navigator
        screenOptions={
            {
                ...stackConfig
            }
        }
    >
        <AddTest.Screen
            options={
                { title: "Создание Теста" }
            }
            screenProps={
                { aaa: "aaa" }
            }
            name="AddTest"
            component={AddTestMain}
        >

        </AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "Единственный вариант" }
            }
            name="AddSingleQuestion"
            component={AddSingleQuestion}
        >

        </AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "Множественный выбор" }
            }
            name="AddMutlipleQuestion"
            component={AddMutlipleQuestion}
        ></AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "Вписать текст" }
            }
            name="AddInsertQuestion"
            component={AddInsertQuestion}
        ></AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "Расположить по порядку" }
            }
            name="AddOrderQuestion"
            component={AddOrderQuestion}
        ></AddTest.Screen>
    </AddTest.Navigator>)
}
export const AddNavigator = ({ navigation, showHeader = true, }) => {
    console.log(showHeader)
    return (<Add.Navigator screenOptions={{ ...stackConfig, headerShown: showHeader }}>
        <Add.Screen
            options={
                {
                    backgroundColor: THEME.colors.primary,
                    title: "Создать новый",
                    headerLeft: () => (
                        <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                            <Item title="Open menu" iconName='ios-menu' onPress={() => navigation.toggleDrawer()}></Item>
                        </HeaderButtons>
                    ),
                }
            }
            name="Add"
            component={BookMarkedScreen} />
        <Add.Screen options={
            {
                title: "Добавить текст",
            }
        } name="AddText" component={AddText} />
        <Add.Screen options={
            {
                title: "Добавить видео",
            }
        } name="AddVideo" component={AddVideo} />
        <Add.Screen options={
            {
                title: "Добавить QR",
            }
        } name="AddQR" component={AddQR} />
        <Add.Screen options={
            {
                title: "Добавить геоточку",
            }
        } name="AddMap" component={AddMap} />
        <Add.Screen options={
            {
                headerShown: false,
                title: "Добавить тест",
            }
        } name="AddTest" component={AddTestNavigator} />
    </Add.Navigator>)
}
export const PostNavigator = ({ navigation, route }) => {
    return <Stack.Navigator
        screenOptions={
            stackConfig
        }
    >
        <Stack.Screen
            options={
                {
                    title: "Мои квесты",
                    headerLeft: () => (
                        <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                            <Item title="Take photo" iconName='ios-menu' onPress={() => navigation.toggleDrawer()}></Item>
                        </HeaderButtons>
                    ),
                }
            }
            name="Main"
            component={MainScreen}
        />
        <Stack.Screen
            options={
                {

                    title: "Обновить квест",
                }
            }
            name="UpdateScreen"
            screenProps={{ itemId: 42 }}
            children={() => <AddNavigator showHeader={false}></AddNavigator>} />
        <Stack.Screen
            options={
                {
                    title: "Отследить квест",
                }
            }
            name="WatchScreen"
            component={WatchScreen} />
        <About.Screen
            options={
                {
                    title: "Предпросмотр",
                    backgroundColor: THEME.colors.primary,
                }
            }
            name="PreviewScreen"
            component={PreviewScreen}
        />
        <About.Screen
            options={
                {
                    title: "Прохождение",
                    backgroundColor: THEME.colors.primary,
                }
            }
            name="GoQuest"
            component={GoScreen}
        />
    </Stack.Navigator>
}



export const BottomNavigator = ({ navigation }) => {
    return (
        <Tab.Navigator
            barStyle={{ backgroundColor: THEME.colors.primary }}
            shifting={true}
            screenOptions={({ route }) => ({
                headerShown: false,
                tabBarIcon: ({ focused, color }) => {
                    let iconName;
                    color = focused ? THEME.PRIMARY_COLOR : "#fff"
                    if (route.name === 'Мои') {
                        iconName = 'checkmark-done-outline'
                    } else if (route.name === 'Add') {
                        iconName = focused ? 'add-circle' : 'add-circle-outline';
                    }
                    return <Ionicons name={iconName} size={20} color={color} />;
                },
            })
            }
        >
            <Tab.Screen
                options={{
                    tabBarLabel: <Text style={{ color: "#fff" }}>Мои</Text>,
                }}
                tabBarLabel="Мои"
                name="Мои"
                component={PostNavigator}
            />
            <Tab.Screen options={{
                tabBarLabel: <Text style={{ color: "#fff" }}>Добавить</Text>,
            }}
                name="Add"
                component={AddNavigator}
            />
        </Tab.Navigator>
    )
}

export const AboutNavigator = ({ navigation }) => <About.Navigator
    screenOptions={
        {
            ...stackConfig,
        }
    } >
    <About.Screen
        options={
            {
                title: "Поиск квестов",
                backgroundColor: THEME.colors.primary,
                headerLeft: () => (
                    <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                        <Item title="Take photo" iconName='ios-menu' onPress={() => navigation.toggleDrawer()}></Item>
                    </HeaderButtons>
                ),
            }
        }
        name="About"
        component={AboutScreen}
    />
    <About.Screen
        options={
            {
                title: "Предпросмотр",
                backgroundColor: THEME.colors.primary,
            }
        }
        name="QuestPreview"
        component={PreviewScreen}
    />
    <About.Screen
        options={
            {
                title: "Прохождение",
                backgroundColor: THEME.colors.primary,
            }
        }
        name="GoQuest"
        component={GoScreen}
    />
</About.Navigator>

export const CreateNavigator = ({ navigation }) => <Create.Navigator
    screenOptions={
        {
            ...stackConfig,
        }
    }>
    <Create.Screen
        options={
            {
                backgroundColor: THEME.colors.primary,
                title: "Профиль",
                headerLeft: () => (
                    <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                        <Item title="Take photo" iconName='ios-menu' onPress={() => navigation.toggleDrawer()}></Item>
                    </HeaderButtons>
                ),
            }
        }
        name="Create"
        component={CreateScreen} />
</Create.Navigator>

export const DrawerNavigation = () => <Drawer.Navigator
    drawerContent={(props) => {
        return (
            <SafeAreaView style={{ flex: 1 }}>
                <View
                    style={{
                        height: 200,
                        alignItems: "flex-start",
                        marginLeft: 15,
                        justifyContent: "center",
                    }}
                >
                    <Image
                        style={{ width: 150, resizeMode: 'contain' }} source={require("../../assets/logo.png")} />
                </View>
                <DrawerItemList {...props} />
            </SafeAreaView>
        );
    }}
    drawerActiveTintColor="#8e8e8f" screenOptions={
        {
            headerShown: false,
            drawerType: "slide",
            drawerActiveTintColor: THEME.colors.primary,
            drawerLabelStyle: {
                fontFamily: 'bold'
            }
        }
    }
>
    <Drawer.Screen
        name="PostTabs"
        options={
            {
                title: 'Мои квесты',
            }
        }
        component={BottomNavigator} />
    <Drawer.Screen
        name="About"
        options={
            {
                title: 'Найти квест',
            }
        }
        component={AboutNavigator} />
    <Drawer.Screen
        name="Create"
        options={
            {
                title: 'Профиль',
            }
        }
        component={CreateNavigator} />
</Drawer.Navigator>

export const InitialNavigation = () => (
    <InitialStack.Navigator
        screenOptions={
            {
                ...stackConfig,
                animation: "flip",
                presentation: "card"
            }
        } >
        <InitialStack.Screen
            options={
                {
                    headerShown: false,
                }
            }
            name="LoginScreen"
            component={LoginScreen}
        />
        <InitialStack.Screen
            options={
                {
                    headerShown: false,
                }
            }
            name="RegisterScreen"
            component={RegisterScreen}
        />
        <InitialStack.Screen
            options={
                {
                    headerShown: false,
                }
            }
            name="DrawerNavigation"
            component={DrawerNavigation}
        />
    </InitialStack.Navigator>
)

export default AppNavigation = () => <NavigationContainer>
    <InitialNavigation></InitialNavigation>
</NavigationContainer>

