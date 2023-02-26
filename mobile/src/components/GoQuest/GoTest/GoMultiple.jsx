import React, { useEffect, useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Card, Text, Checkbox } from "react-native-paper"

const GoMultiple = ({ question, index, onAnswerChange }) => {
    const [answer, setAnswer] = useState([question.answers[0]])

    useEffect(() => {
        onAnswerChange(index, answer)
    }, [answer])


    const onSetAnswer = (item) => {
        let res = [...answer]
        const index = answer.indexOf(item)
        if (index == -1) {
            res = [...new Set([...res, item])]
        } else {
            res.splice(index, 1)
        }
        setAnswer(res)
    }
    return (
        <Card style={styles.card}>
            {
                question.answers.map((item, key) => (
                    <Checkbox.Item
                        position='trailing'
                        labelVariant='bodyLarge'
                        label={item}
                        style={{ height: 45 }}
                        status={answer.indexOf(item) !== -1 ? "checked" : "unchecked"}
                        onPress={(e) => onSetAnswer(item)}
                        value={answer}
                    />
                ))
            }
        </Card>

    );
}

const styles = StyleSheet.create({
    card: {
        width: "98%",
        margin: 4,
        flex: 1,
        height: " 100%",
        borderRadius: 4
    }
})

export default GoMultiple;
