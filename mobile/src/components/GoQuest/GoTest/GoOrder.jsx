import React, { useEffect, useState } from 'react';
import { View, StyleSheet, TouchableOpacity, TouchableNativeFeedback  } from 'react-native';
import { Card, Text, RadioButton, TextInput, Surface, TouchableRipple } from "react-native-paper"
import DraggableFlatList from 'react-native-draggable-dynamic-flatlist'
import { THEME } from '../../../theme';

renderItem = ({ item, index, move, moveEnd, isActive }) => {
    return (
        <TouchableNativeFeedback
            style={{
                
                alignItems: 'center',
                justifyContent: 'center'
            }}
            onLongPress={move}
            onPressOut={moveEnd}>
            <Surface style={{...styles.surface, backgroundColor: isActive ? THEME.colors.surfaceVariant : THEME.colors.surface,}} elevation={1}>
                <Text style={{
                    fontWeight: 'bold',
                    color: THEME.colors.primary,
                    fontSize: 12,
                }}>{item}</Text>
            </Surface>

        </TouchableNativeFeedback>
    )
}

const GoOrder = ({ question, index, onAnswerChange }) => {
    const [answer, setAnswer] =
        useState(question
            .rightAnswer
            .map(value => ({ value, sort: Math.random() }))
            .sort((a, b) => a.sort - b.sort)
            .map(({ value }) => value))

    useEffect(() => {
        onAnswerChange(index, answer)
    }, [answer])

    return (
        <View style={styles.card}>

            <DraggableFlatList
                data={answer}
                renderItem={renderItem}
                keyExtractor={(item, index) => `draggable-item-${item}`}
                scrollPercent={5}
                onMoveEnd={({ data }) => setAnswer(data)}
            />
        </View>
    );
}

const styles = StyleSheet.create({
    card: {
        width: "98%",
        margin: 4,
        borderRadius: 4
    },
    surface: {
        padding: 8,
        height: 40,
        width: "98%",
        margin: 4,
        alignItems: 'center',
        justifyContent: 'center',
    },
})

export default GoOrder;
