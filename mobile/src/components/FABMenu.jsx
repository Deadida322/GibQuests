import React from 'react';
import { StyleSheet } from 'react-native';
import { FAB } from 'react-native-paper';

const FABMenu = ({ show, onStateChange, onOpenStage }) => {

    return (
        <FAB.Group
            style={styles.fab}
            open={show}
            visible
            variant="secondary"
            icon={show ? 'pencil' : 'plus'}
            actions={[
                {
                    icon: 'card-text-outline',
                    label: 'Текст',
                    onPress: () => onOpenStage("text"),
                },
                {
                    icon: 'youtube',
                    label: 'Видео',
                    onPress: () => onOpenStage("video"),
                },
                {
                    icon: 'qrcode',
                    label: 'QR',
                    onPress: () => onOpenStage("qrcode"),
                },
                {
                    icon: 'map-outline',
                    label: 'Карта',
                    onPress: () => onOpenStage("map"),
                },
                {
                    icon: 'school-outline',
                    label: 'Тест',
                    onPress: () => onOpenStage("test"),
                },
            ]}
            onStateChange={onStateChange}
        />
    );
}


const styles = StyleSheet.create({
    fab: {
        position: "absolute",
        margin: 2,
        width: "100%",
        right: -1,
    },
})

export default FABMenu;
