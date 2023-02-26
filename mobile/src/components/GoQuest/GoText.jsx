import { Button, Title, Paragraph, Text, Card, Chip } from 'react-native-paper';
import { View, StyleSheet } from 'react-native';
import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';

export default function GoText({ stage, index, onNextStage }) {
    const goTo = useTabNavigation();
    const idx = useTabIndex()
    const transition = () => {
        onNextStage(idx + 1)
        goTo(idx + 1)
    }
    return (
        <Card style={styles.card}>
            <Text style={styles.chip}><Chip icon="card-text-outline">Этап {index + 1}</Chip></Text>

            <Card.Title title={stage.title} />

            <Card.Content>
                <Text variant="bodyMedium">{stage.description}</Text>
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
        marginTop: 40
    },
    title: {
        marginBottom: -4
    },
    chip: {
        position: "absolute",
        top: -15,
        right: 0,
    }
})
