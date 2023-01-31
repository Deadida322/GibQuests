import React from 'react';
import {View, StyleSheet, Text, ImageBackground, TouchableOpacity, TouchableNativeFeedback} from 'react-native';

const Post = ({post, onPress}) => {
    return (
        <TouchableOpacity onPress={()=>onPress()} activeOpacity={.7} style={styles.post}>
            <View>
                <ImageBackground style={{height: 150}} source={{uri: post.img}}>
                    <View style={styles.textWrap}>
                        <Text style={styles.title}>{new Date().toLocaleDateString(post.date)}</Text>
                    </View>
                </ImageBackground>
            </View>
        </TouchableOpacity>
    );
}

const styles = StyleSheet.create({
    post:{
        backgroundColor: "white",
        margin: 5,
        overflow: 'hidden',
        borderRadius: 6,
        elevation: 4,
    },
    textWrap:{
        padding: 10,
        alignItems: "center",
        width: "100%",
        backgroundColor: "#0000006f"
    },
    title: {
        color: "#FFF",
        fontFamily: 'regular'
    }
})

export default Post;
