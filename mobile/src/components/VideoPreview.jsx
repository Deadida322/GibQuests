import React, {useEffect, useState} from 'react';
import { StyleSheet, View, Text} from 'react-native';
import {Card } from 'react-native-paper';
import { WebView } from 'react-native-webview';

const VideoPreview = ({ stage, onVideoReload}) => {
    const [reformedUri, setReformedUri] = useState("");
    useEffect(()=>{
        const splited = stage.url.split("/")
        setReformedUri("https://www.youtube.com/embed/" + splited[splited.length-1])
    }, [stage])

    return (
        <>
            <Card style={styles.card}>
                <Card.Title title={stage.title || "Видео этап"} />
                <Card.Content>
                    <View style={{ width: "100%", height: 200 }}>
                        <WebView
                            originWhitelist={['http://*', 'https://*', 'about:srcdoc']}
                            scrollEnabled
                            source={{ uri: reformedUri }}
                            style={{ borderWidth: 1, borderColor: 'red', flex: 1 }}
                        />
                    </View>
                </Card.Content>
            </Card>
        </>
    );
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 2,
        marginTop: 8,
        marginBottom: 4
    },
    webview: {
        height: 400,
        width: 100
    }
})

export default VideoPreview;
