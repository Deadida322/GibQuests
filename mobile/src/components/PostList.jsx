import React from 'react';
import {View, StyleSheet, FlatList} from 'react-native';
import Post from './Post';

const PostList = ({data, onOpen}) => {
    return (
        <View style={styles.wrapper}>
            <FlatList 
                data={data} 
                style={styles.center}
                keyExtractor={(post)=>post.id.toString()}
                renderItem={({item})=><Post post={item} onPress={()=>onOpen(item)}></Post>}
            />
        </View>
    );
}

const styles = StyleSheet.create({
    wrapper: {
        padding: 10
    }
})

export default PostList;
