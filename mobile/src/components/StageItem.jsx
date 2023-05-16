import { StyleSheet } from 'react-native';
import { Card, Button, Avatar } from "react-native-paper"
import { MaterialCommunityIcons } from '@expo/vector-icons';
import { THEME } from "../theme"

const getIcon = (type) => {
    switch (type) {
        case "text": {
            return 'card-text-outline'
        }
        case "video": {
            return 'youtube'
        }
        case "qrcode": {
            return 'qrcode'
        }
        case "qrCode": {
            return 'qrcode'
        }
        case "map": {
            return 'map-outline'
        }
        case "test": {
            return 'school-outline'
        }
    }
}

const getSubtitle = (stage) => {
    switch (stage.type) {
        case "text": {
            return stage.text
        }
        case "video": {
            return stage.url
        }
        case "qrCode": {
            return stage.code
        }
        case "qrcode": {
            return stage.code
        }
        case "map": {
            return `${stage.coords.latitude};${stage.coords.longitude}`
        }
        case "test": {
            return `${stage.questions.length} вопросов`
        }
    }
}


const StageItem = ({ stage, onStageEdit, onStageDelete, editable }) => {

    return stage ? (
        <Card style={styles.card}>
            <Card.Title title={stage.title}
                left={() => <Avatar.Icon size={42} icon={getIcon(stage.type)}></Avatar.Icon>} subtitle={getSubtitle(stage)}
            />
            <Card.Content>

            </Card.Content>
            { editable ??
                <Card.Actions>
                    <Button onPress={() => onStageEdit()} mode="elevated"><MaterialCommunityIcons name="pencil" size={22} color={THEME.colors.primary} /></Button>
                    <Button onPress={() => onStageDelete()} mode="elevated" dark={true} buttonColor={THEME.colors.error}><MaterialCommunityIcons name="delete" size={22} /></Button>
                </Card.Actions>
            }
        </Card>
    ) : "";
}

const styles = StyleSheet.create({
    card: {
        borderRadius: 2,
        marginBottom: 10
    }
})

export default StageItem;
