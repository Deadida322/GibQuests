import { Button, Title, Paragraph, Text, Card, Chip } from 'react-native-paper';
import { View, StyleSheet, ScrollView } from 'react-native';
import { useState } from 'react';
import { Tabs, TabScreen, useTabIndex, useTabNavigation } from 'react-native-paper-tabs';
import { ProgressSteps, ProgressStep } from 'react-native-progress-steps';
import GoSingle from './GoSingle';
import GoMultiple from './GoMultiple';
import { THEME } from '../../../theme';
import GoInsert from './GoInsert';
import GoOrder from './GoOrder';

export default function GoTest({ stage, index, onNextStage }) {
    const [answers, setAnswers] = useState([])
    const [currentStep, setCurrentStep] = useState(0)
    const goTo = useTabNavigation();
    const idx = useTabIndex()

    const transition = () => {
        onNextStage(idx + 1)
        goTo(idx + 1)
    }

    const onAnswerChange = (index, answer) => {
        setAnswers((prev) => {
            let arr = [...prev]
            arr[index] = answer
            console.log(arr)
            return arr
        })
    }

    const getTooltip = (question) => {
        switch (question.type) {
            case ("single"): {
                return <Text variant='labelSmall'>Выберите один вариант</Text>
            }
            case ("multiple"): {
                return <Text variant='labelSmall'>Выберите несколько вариантов</Text>
            }
            case ("insert"): {
                return <Text variant='labelSmall'>Вставьте ответ</Text>
            }
            case ("order"): {
                return<Text variant='labelSmall'>Расположите по порядку</Text>
            }
        }
    }

    const getGoComponent = (question, index) => {
        switch (question.type) {
            case ("single"): {
                return <GoSingle onAnswerChange={onAnswerChange} question={question} index={index}></GoSingle>
            }
            case ("multiple"): {
                return <GoMultiple onAnswerChange={onAnswerChange} question={question} index={index}></GoMultiple>
            }
            case ("insert"): {
                return <GoInsert onAnswerChange={onAnswerChange} question={question} index={index}></GoInsert>
            }
            case ("order"): {
                return <GoOrder onAnswerChange={onAnswerChange} question={question} index={index}></GoOrder>
            }
        }
    }

    return (
        <Card style={styles.card}>
            <Text style={styles.chip}>
                <Chip icon="school-outline">Этап {index + 1}</Chip>
            </Text>
            <Card.Title title={stage.title} />
            <Card.Content style={{ minHeight: "100%" }}>
                <ProgressSteps
                    activeStep={currentStep}
                    borderWidth={3}
                    completedStepIconColor={THEME.colors.primary}
                    completedProgressBarColor={THEME.colors.primary}
                    activeStepIconBorderColor={THEME.colors.primary}
                    activeLabelColor={THEME.colors.primary}
                    topOffset={0}
                    marginBottom={10}
                >
                    {stage.questions.map(
                        (question, index) => (
                            <ProgressStep removeBtnRow={true} scrollable={false}>
                                <Text variant='titleMedium' style={{ color: THEME.colors.primary }}>{question.title}</Text>
                                <ScrollView style={{ flex: 1, marginTop: 8, marginBottom: 4 }}>
                                    {getGoComponent(question, index)}
                                </ScrollView>
                                {getTooltip(question)}  
                                <View style={styles.buttonsRow}>
                                    <Button
                                        disabled={currentStep === 0}
                                        onPress={() => setCurrentStep(currentStep - 1)}
                                        style={{ marginRight: 4 }}
                                        mode="contained-tonal"
                                    >
                                        Предыдущий
                                    </Button>
                                    <Button
                                        disabled={currentStep === stage.questions.length - 1}
                                        onPress={() => setCurrentStep(currentStep + 1)}
                                        mode="contained"
                                    >
                                        Следующий
                                    </Button>
                                </View>
                            </ProgressStep>)
                    )}
                </ProgressSteps>
            </Card.Content>
            <Card.Actions style={styles.actions}>
                <Button>Назад</Button>
                <Button
                    disabled={currentStep !== stage.questions.length - 1}
                    onPress={() => transition()}
                >
                    Далее
                </Button>
            </Card.Actions>
        </Card>
    )
}

const styles = StyleSheet.create({
    card: {
        marginTop: 20,
        borderRadius: 6,
        height: "80%",
        paddingBottom: 20,
        margin: 6
    },
    title: {
        marginBottom: -4
    },
    chip: {
        position: "absolute",
        top: -15,
        right: 0,
    },
    buttonsRow: {
        display: 'flex',
        flexDirection: "row",
        marginTop: 8,
        justifyContent: "flex-end",
        marginBottom: 40
    },
    actions: {
        marginBottom: 70
    }
})
