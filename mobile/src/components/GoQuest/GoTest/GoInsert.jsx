import React, { useEffect, useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Card, Text, RadioButton, TextInput } from "react-native-paper"

const GoInsert = ({ question, index, onAnswerChange }) => {
    const [answer, setAnswer] = useState([""])

    useEffect(() => {
        onAnswerChange(index, answer)
    }, [answer])

    return (
        <Card style={styles.card}>
            <Card.Content>
                <TextInput
                    label="Ваш ответ"
                    mode="outlined"
                    value={answer[0]}
                    onChangeText={text => setAnswer([text])}
                />
            </Card.Content>
        </Card>
    );
}

const styles = StyleSheet.create({
    card: {
        width: "98%",
        margin: 4,
        borderRadius: 4
    }
})

export default GoInsert;
