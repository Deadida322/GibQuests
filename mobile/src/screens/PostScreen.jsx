import React, {useEffect, useState} from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { removePost, toggleBooked } from '../store/actions/post';
import {View, Text, StyleSheet, Image, ScrollView, Button, Alert} from 'react-native';
import { HeaderButtons, Item } from 'react-navigation-header-buttons';
import AppHeaderIcon from '../components/AppHeaderIcon';
import { DATA } from '../data';
import { THEME } from '../theme';

const PostScreen = ({route, navigation}) => {
    const dispatch = useDispatch()
    const postId = route.params.postId
    let post = useSelector(state=>state.post.allPosts.find(item=>item.id==postId))
    let [booked, setBooked] = useState(post?.booked)
    let [iconName, setIconName] = useState('ios-star')
    
    const removeHandler = ()=>{
        Alert.alert(
            "Нихуясебебля",
            "Ты че удаляешь?",
            [
              {
                text: "э",
                onPress: () => {
                    dispatch(removePost(postId))
                    navigation.navigate('Main')
                }
              },
              {
                text: "Твоюмать",
                onPress: () => console.log("Cancel Pressed"),
                style: "cancel"
              },
            ]
          );
    }
    useEffect(() => {
        setIconName(booked ?'star': 'ios-star-outline')
        navigation.setOptions(
            {
                title: `Пост ${postId}`,
                headerRight: () => (
                    <HeaderButtons HeaderButtonComponent={AppHeaderIcon}>
                        <Item title="Take photo" iconName={iconName} onPress={()=>{
                            dispatch(toggleBooked(post))
                            setBooked(prev=>!prev)
                        }}></Item>
                    </HeaderButtons>
                ),
            })
    }, [booked, iconName]);
    if (!post) return null

    return (
        <ScrollView style={styles.post}>
            <Image source={{uri:post.img}} style={styles.image}/>
            <View>
                <Text style={styles.title}>{post.title}</Text>
            </View>
            <View style={styles.textWrapper}>
                <Text>{post.text.repeat(10)}</Text>
            </View>
            <View style={styles.textWrapper}>
                <Button onPress={removeHandler} title="удалить" color={THEME.DANGER_COLOR}></Button>
            </View>
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    post: {
        padding: 6
    },
    image:{
       width: "100%",
       borderRadius: 6,
       height: 200
    },

    title: {
        marginTop: 10,
        textAlign: "center",
        fontFamily: "bold",
        fontSize: 20
    },
    textWrapper:{
        marginVertical: 10,
        alignItems: "center"
    }
})

export default PostScreen;
