import React from 'react';
import { StyleSheet } from 'react-native';
import { FAB } from 'react-native-paper';

const AddTestFAB = ({ show, onStateChange, onOpenQuestion }) => {

    return (
        <FAB.Group
            style={styles.fab}
            open={show}
            visible
            variant="secondary"
            icon={show ? 'pencil' : 'plus'}
            actions={[
                {
                    icon: 'help',
                    label: 'Один ответ',
                    onPress: () => onOpenQuestion("single"),
                },
                {
                    icon: 'beaker-question-outline',
                    label: 'Множественный выбор',
                    onPress: () => onOpenQuestion("multiple"),
                },
                {
                    icon: 'content-paste',
                    label: 'Вписать ответ',
                    onPress: () => onOpenQuestion("insert"),
                },
                {
                    icon: 'reorder-horizontal',
                    label: 'Расположить по порядку',
                    onPress: () => onOpenQuestion("order"),
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
        zIndex: 999
    },
})

export default AddTestFAB;
