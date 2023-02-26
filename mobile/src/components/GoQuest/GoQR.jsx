import { Button, Title, Paragraph, Text, Card, Chip } from 'react-native-paper';
import { View, StyleSheet, Alert } from 'react-native';
import { useState, useEffect } from 'react';
import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';
import { BarCodeScanner } from 'expo-barcode-scanner';

export default function GoQR({ stage, index, onNextStage }) {
    const [hasPermission, setHasPermission] = useState(null);
    const [scanned, setScanned] = useState(false);
    const [goScan, setGoScan] = useState(false);
    const goTo = useTabNavigation();
    const idx = useTabIndex()
    const transition = () => {
        onNextStage(idx + 1)
        goTo(idx + 1)
        setGoScan(false)
    }
    useEffect(() => {
        (async () => {
            const { status } = await BarCodeScanner.requestPermissionsAsync();
            setHasPermission(status === 'granted');
        })();
    }, []);

    const handleBarCodeScanned = ({ type, data }) => {
        Alert.alert(
            "Код просканирован",
            `Кодовое слово ${data}`,
            [
                "Дальше"
            ]
        );
        setScanned(true);
    };

    return (
        <Card style={styles.card}>
            <Text style={styles.chip}><Chip icon="qrcode">Этап {index + 1}</Chip></Text>
            <Card.Title title={stage.title} />

            <Card.Content>

                <Text variant="bodyMedium">Отсканируйте QRCode</Text>
                {goScan ?
                    <BarCodeScanner
                        onBarCodeScanned={scanned ? undefined : handleBarCodeScanned}
                        style={styles.qr}
                    /> :
                    <Button icon="qrcode" style={{ margin: 8 }} mode="contained-tonal" onPress={() => setGoScan(true)}>
                        Сканировать
                    </Button>
                }
            </Card.Content>
            <Card.Actions>
                <Button>Назад</Button>
                <Button disabled={!scanned} onPress={() => transition()}>Далее</Button>
            </Card.Actions>
        </Card>
    )
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 4,
        margin: 6,
        marginTop: 20
    },
    title: {
        marginBottom: -4
    },
    qr: {
        height: 320,
        marginTop: 8
    },
    chip: {
        position: "absolute",
        top: -15,
        right: 0,
    }
})
