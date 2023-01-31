import React, {useEffect, useState} from 'react';
import { StyleSheet, View, Text} from 'react-native';
import { Avatar, Button, Card } from 'react-native-paper';
import { MaterialCommunityIcons } from '@expo/vector-icons';
import { THEME } from '../theme';
import { WebView } from 'react-native-webview';


const VideoPreview = ({ stage}) => {
    return (
        <>
            <Card style={styles.card}>
                <Card.Title title={stage.title || "Текстовый этап"}></Card.Title>
                <Card.Content>
                    <Text>
                        {stage.description || "Описание этапа"}
                    </Text>
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
