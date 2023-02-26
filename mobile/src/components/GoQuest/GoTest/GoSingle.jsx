import React, { useEffect, useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Card, Text, RadioButton } from "react-native-paper"

const GoSingle = ({ question, index, onAnswerChange }) => {
    const [answer, setAnswer] = useState([question.answers[0]])

    useEffect(()=>{
        onAnswerChange(index, answer)
    }, [answer])

    return (
        <Card style={styles.card}>
            <Card.Content>
                <RadioButton.Group value={answer[0]} onValueChange={newVal => setAnswer([newVal])}>
                    {
                        question.answers.map((item, key) => (
                            <RadioButton.Item
                                position='trailing'
                                labelVariant='bodyLarge'
                                label={item}
                                value={item} 
                            />
                        ))
                    }
                </RadioButton.Group>
            </Card.Content>
        </Card>
    );
}

const styles = StyleSheet.create({
    card: {
        width: "98%",
        borderRadius: 4,
        margin: 4,
    }
})

export default GoSingle;
