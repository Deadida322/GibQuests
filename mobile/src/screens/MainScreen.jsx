import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { StyleSheet, ActivityIndicator, Text, View } from "react-native";
import PostList from "../components/PostList";
import { loadPosts } from "../store/actions/post";
import { FAB, Button } from "react-native-paper";
import FABMenu from "../components/FABMenu";

export default function MainScreen({ navigation }) {
    const dispatch = useDispatch();
    let allPosts = [];
    function goToPost(post) {
        navigation.navigate("Post", { postId: post.id });
    }
    allPosts = useSelector((state) => state.post.allPosts);
    const loading = useSelector((state) => state.post.loading);

    useEffect(() => {
        console.log("sadasd");
        dispatch(loadPosts());
    }, []);

    if (loading)
        return (
            <View style={styles.center}>
                <ActivityIndicator></ActivityIndicator>
                <Text>Загрузка...</Text>
            </View>
        );
    return (
        <>
            <PostList onOpen={goToPost} data={allPosts}></PostList>
            <Button
                icon="camera"
                mode="outlined"
                onPress={() => console.log("Pressed")}
            >
                Press me
            </Button>
        </>
    );
}

const styles = StyleSheet.create({
    center: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
    },
    fab: {
        position: "absolute",
        margin: 16,
        right: 0,
        bottom: 0,
    },
});
