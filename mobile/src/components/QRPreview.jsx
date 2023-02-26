import React, { useEffect, useState } from 'react';
import { StyleSheet, View, ToastAndroid } from 'react-native';
import Ionicons from '@expo/vector-icons/Ionicons';

import { Card, Button, Banner, Text } from 'react-native-paper';
import * as MediaLibrary from 'expo-media-library';
import * as FileSystem from 'expo-file-system';
import QRCode from 'react-native-qrcode-svg';
import { THEME } from '../theme';

const QRPreview = ({ stage }) => {
    const [svg, setSvg] = useState("");

    async function saveQrToDisk() {
        svg.toDataURL(async (data) => {
            const filename = FileSystem.documentDirectory + "QR.png";
            await FileSystem.writeAsStringAsync(filename, data, {
                encoding: FileSystem.EncodingType.Base64,
            });
            MediaLibrary.requestPermissionsAsync().then(() => {
                MediaLibrary.saveToLibraryAsync(filename).then(() => {
                    ToastAndroid.show("Файл был сохранён", 4000)
                })
            })

        })
    }
    return (
        <>
            <Card style={styles.card}>
                <Card.Title title={stage.title || "Этап с кодом"}></Card.Title>
                <Card.Content>
                    <View style={styles.qrContainer}>
                        {
                            stage.code ? <QRCode getRef={(c) => setSvg(c)} size={300} value={stage.code}></QRCode> :
                                <Text style={{ color: THEME.colors.secondary }}>Здесь отобразится QR после ввода кодового слова</Text>
                        }
                    </View>
                </Card.Content>
                <Card.Actions>
                    <Button disabled={!stage.code} onPress={saveQrToDisk} mode="contained" icon="download">Скачать</Button>
                </Card.Actions>
            </Card>
        </>
    );
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 2,
        margin: 2,
        marginBottom: 40
    },
    qr: {
        width: "100%"
    },
    qrContainer: {
        width: "100%",
        alignItems: "center"
    }
})

export default QRPreview;
