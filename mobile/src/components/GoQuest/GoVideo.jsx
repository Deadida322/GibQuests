import { Button, Title, Paragraph, Text, Card, Chip } from 'react-native-paper';
import { View, StyleSheet } from 'react-native';
import { WebView } from 'react-native-webview';
import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';

export default function GoVideo({ stage, index, onNextStage }) {
    const goTo = useTabNavigation();
    const idx = useTabIndex()
    const transition = () => {
        onNextStage(idx + 1)
        goTo(idx + 1)
    }
    const getUri = () => {
        const splited = stage.url.split("/")
        return "https://www.youtube.com/embed/" + splited[splited.length - 1]
    }
    return (
        <Card style={styles.card}>
            <Text style={styles.chip}><Chip icon="youtube">Этап {index + 1}</Chip></Text>

            <Card.Title title={stage.title} />

            <Card.Content>
                <View style={{ width: "100%", height: 200 }}>
                    <WebView
                        // fixes warning when page has an iframe (about:srcdoc)
                        originWhitelist={['http://*', 'https://*', 'about:srcdoc']}
                        // renderLoading={this._renderLoadingWebView}
                        scrollEnabled
                        source={{ uri: getUri(stage.url) }}
                        style={{ borderWidth: 1, borderColor: 'red', flex: 1 }}
                    />
                </View>
            </Card.Content>
            <Card.Actions>
                <Button>Назад</Button>
                <Button onPress={() => transition()}>Далее</Button>
            </Card.Actions>
        </Card>
    )
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 6,
        margin: 4,
        marginTop: 20
    },
    title: {
        marginBottom: -4
    },
    chip:{
        position: "absolute",
        top: -15,
        right: 0,
    }
})
