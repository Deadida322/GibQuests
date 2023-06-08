export const state = () => {
    return {
        currentStage: {},
        currentQuest: {
            id: "new",
            stages: [],
        },
    };
};

export const mutations = {
    setCurrentStage(state, stage) {
        state.currentStage = JSON.parse(JSON.stringify(stage));
    },
    removeCurrentQuest(state) {
        state.currentQuest = {
            title: "Новый квест",
            id: "new",
            stages: [],
            policy: {
                policyType: "public",
                memberType: "group",
            },
        };
    },
    setCurrentQuest(state, quest) {
        state.currentQuest = {
            ...JSON.parse(JSON.stringify(quest)),
            image: quest.image,
            stages: quest.stages.map((val, idx) => ({
                order: idx,
                ...val,
            })),
        };
    },
};

export const getters = {
    getCurrentStage: (s) => JSON.parse(JSON.stringify(s.currentStage)),
    getCurrentQuest: (s) => {
        return {
            ...JSON.parse(JSON.stringify(s.currentQuest)),
            image: s.currentQuest.image,
        };
    },
};
