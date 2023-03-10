import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { HeaderButtons, Item } from 'react-navigation-header-buttons'
import AppHeaderIcon from '../components/AppHeaderIcon';
import MainScreen from '../screens/MainScreen';
import PostScreen from '../screens/PostScreen';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs';
import { THEME } from '../theme';
import BookMarkedScreen from '../screens/BookMarkedScreen'
import { Ionicons } from '@expo/vector-icons';
import { createDrawerNavigator } from '@react-navigation/drawer';
import AboutScreen from '../screens/AboutScreen'
import { Text } from 'react-native-paper';
import CreateScreen from '../screens/CreateScreen'
import AddText from '../screens/Edit/AddText';
import AddVideo from "../screens/Edit/AddVideo"
import AddQR from "../screens/Edit/AddQR"
import AddMap from '../screens/Edit/AddMap';
import AddTestMain from '../screens/AddTest/AddTestMain';
import AddSingleQuestion from '../screens/AddTest/AddSingleQuestion';
import AddMutlipleQuestion from '../screens/AddTest/AddMultipleQuestion'
import AddInsertQuestion from '../screens/AddTest/AddInsertQuestion'
import AddOrderQuestion from  '../screens/AddTest/AddOrderQuestion'
import { addAssetsToAlbumAsync } from 'expo-media-library';

const Drawer = createDrawerNavigator();
const Stack = createNativeStackNavigator();
const Add = createNativeStackNavigator();
const Tab = createMaterialBottomTabNavigator()
const Create = createNativeStackNavigator();
const About = createNativeStackNavigator();
const AddTest = createNativeStackNavigator();

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
                { title: "???????????????? ??????????" }
            }
            screenProps ={
               { aaa: "aaa"}
            }
            name="AddTest"
            component={AddTestMain}
        >

        </AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "???????????????????????? ??????????????" }
            }
            name="AddSingleQuestion"
            component={AddSingleQuestion}
        >

        </AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "?????????????????????????? ??????????" }
            }
            name="AddMutlipleQuestion"
            component={AddMutlipleQuestion}
        ></AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "?????????????? ??????????" }
            }
            name="AddInsertQuestion"
            component={AddInsertQuestion}
        ></AddTest.Screen>
        <AddTest.Screen
            options={
                { title: "?????????????????????? ???? ??????????????" }
            }
            name="AddOrderQuestion"
            component={AddOrderQuestion}
        ></AddTest.Screen>
    </AddTest.Navigator>)
}

export const PostNavigator = ({ navigation }) => {
    return <Stack.Navigator
        screenOptions={
            {
                ...stackConfig,
                headerRight: () => (
                    <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                        <Item title="Take photo" iconName='camera' onPress={() => navigation.navigate('Create')}></Item>
                    </HeaderButtons>
                ),
            }
        }
    >
        <Stack.Screen
            options={
                {
                    title: "?????? ????????????",
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
        <Stack.Screen name="Post" component={PostScreen} />
    </Stack.Navigator>
}

export const AddNavigator = ({ navigation }) => {
    return (<Add.Navigator screenOptions={stackConfig}>
        <Add.Screen
            options={
                {
                    backgroundColor: THEME.colors.primary,
                    title: "?????????????? ??????????",
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
                title: "???????????????? ??????????",
            }
        } name="AddText" component={AddText} />
        <Add.Screen options={
            {
                title: "???????????????? ??????????",
            }
        } name="AddVideo" component={AddVideo} />
        <Add.Screen options={
            {
                title: "???????????????? QR",
            }
        } name="AddQR" component={AddQR} />
        <Add.Screen options={
            {
                title: "???????????????? ????????????????",
            }
        } name="AddMap" component={AddMap} />
        <Add.Screen options={

            {
                headerShown: false,
                title: "???????????????? ????????",
            }
        } name="AddTest" component={AddTestNavigator} />
    </Add.Navigator>)
}

export const BottomNavigator = () => {
    return (
        <Tab.Navigator
            barStyle={{ backgroundColor: THEME.colors.primary }}
            shifting={true}

            screenOptions={({ route }) => ({
                headerShown: false,
                tabBarIcon: ({ focused, color }) => {
                    let iconName;
                    color = focused ? THEME.PRIMARY_COLOR : "#fff"
                    if (route.name === '??????') {
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
                    tabBarLabel: <Text style={{ color: "#fff" }}>??????</Text>,
                }}
                tabBarLabel="??????"
                name="??????"
                component={PostNavigator}
            />
            <Tab.Screen options={{
                tabBarLabel: <Text style={{ color: "#fff" }}>????????????????</Text>,
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
                title: "???? ??????",
                backgroundColor: THEME.colors.primary,
                headerLeft: () => (
                    <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                        <Item title="Take photo" iconName='ios-menu' onPress={() => navigation.toggleDrawer()}></Item>
                    </HeaderButtons>
                ),
            }
        }
        name="About"
        component={AboutScreen} />
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
                title: "??????????????",
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

export default AppNavigation = () => <NavigationContainer>
    <Drawer.Navigator
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
                    title: '?????? ????????????',
                }
            }
            component={BottomNavigator} />
        <Drawer.Screen
            name="About"
            options={
                {
                    title: '?????????????? ??????????',
                }
            }
            component={AboutNavigator} />
        <Drawer.Screen
            name="Create"
            options={
                {
                    title: '?????? ????????????',
                }
            }
            component={CreateNavigator} />
    </Drawer.Navigator>
</NavigationContainer>

