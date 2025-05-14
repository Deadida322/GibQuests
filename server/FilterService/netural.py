import os
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import OneHotEncoder
import tensorflow as tf
from keras import layers as L, Sequential
import json

from utils import make_dataset, models_dir

os.environ["KERAS_BACKEND"] = "tensorflow"
import keras

from reader import get_texts, get_texts_with_toxic
import time
from stats import get_vocab, get_text_stats
from joblib import dump

#model parameters
name="count-9"
vectorizer="count"
description = "Описание: модель с токсичными комментариями"
encoderName = "oneHot"


# hyper parameters
base_dir = os.getenv('BASE_DIR')
test_split = 0.15
batch_size = 128
auto = tf.data.AUTOTUNE
epochs = 5
vocab_size = 200000
ngrams = 1
embedding_dim = 16

maxlen = 180
# max_words = 5000
filter_length = 300

text_vectorizer = L.TextVectorization(
    max_tokens=vocab_size, ngrams=ngrams, output_mode=vectorizer
)

def make_model(vocab_Label_size = 4):
    model = Sequential([
        L.Dense(512, activation="relu"),
        L.Dense(256, activation="relu"),
        L.Dense(vocab_Label_size, activation="sigmoid"),
    ])
    model.compile(
        loss="binary_crossentropy", optimizer="adam", metrics=["binary_accuracy"]
    )

    # model = Sequential([
    #     L.Embedding(vocab_size, 20, input_length=maxlen),
    #     L.Dropout(0.1),
    #     L.Conv1D(filter_length, 3, padding='valid', activation='relu', strides=1),
    #     L.GlobalMaxPool1D(),
    #     L.Dense(vocab_Label_size, activation="sigmoid"),
    # ])
    #
    # model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['categorical_accuracy'])

    #
    # model.summary()

    return model

class ModelEndtoEnd(keras.Model):
    def predict(self, inputs):
        indices = text_vectorizer(inputs)
        return super().predict(indices)


def get_inference_model(model):
    inputs = model.inputs
    outputs = model.outputs
    end_to_end_model = ModelEndtoEnd(inputs, outputs, name="end_to_end_model")
    end_to_end_model.compile(
        optimizer="adam", loss="binary_crossentropy", metrics=["accuracy"]
    )
    return end_to_end_model

def save_model_params(encoder, model, accuracy):
    subfolders = [f.path for f in os.scandir(models_dir) if f.is_dir()]
    folder_index = len(subfolders) + 1
    folder_path = os.path.join(str(models_dir), str(folder_index))
    os.mkdir(folder_path)
    settings_file_path = os.path.join(str(folder_path), "settings.json")
    model_file_path = os.path.join(str(folder_path), "model.keras")
    encoder_file_path = os.path.join(str(folder_path), "encoder.joblib")

    settings = {
        "name": name,
        "vectorizer": vectorizer,
        "ngrams": ngrams,
        "vocab_size": vocab_size,
        "encoder": encoderName,
        "epochs": epochs,
        "batch_size" : batch_size,
        "description": description,
        "accuracy": accuracy
    }

    with open(settings_file_path, 'w', encoding='utf-8') as f:
        json.dump(settings, f, ensure_ascii=False, indent=4)

    model.save(model_file_path)
    dump(encoder, encoder_file_path)

    print("save keras model")

def train():
    textsWithCategory = get_texts_with_toxic()
    # конверт в датафрейм
    texts_pd = pd.DataFrame(textsWithCategory, columns=["text", "rating"])
    start_time = time.time()
    print('Получили тексты')

    # получение размера словаря всех слов
    # local_vocab_size = get_vocab(texts_pd)
    # if(local_vocab_size > 200000):
    #     vocab_size = 200000
    print("vocab_size: ", vocab_size)

    # кодируем метки
    # делаем форму [[6], [12], [16], [18], ... ]
    arr_labels = np.array(texts_pd["rating"]).reshape(-1, 1)

    # кол-во меток классов
    vocab_size_label = len(np.unique(arr_labels))

    encoder = OneHotEncoder(sparse_output=False, dtype=np.integer)
    encoder.fit(arr_labels)

    # получение статистики по текстам
    # get_text_stats(texts_pd)
    # получение статистики по распределению классов
    # print_classes(texts_pd)

    train_df, test_df = train_test_split(
        texts_pd,
        test_size=test_split,
        random_state=42
    )

    val_df = test_df.sample(frac=0.5)
    test_df.drop(val_df.index, inplace=True)

    train_dataset = make_dataset(train_df, encoder, batch_size, is_train=True)
    validation_dataset = make_dataset(val_df, encoder, batch_size, is_train=False)
    test_dataset = make_dataset(test_df, encoder, batch_size, is_train=False)

    # `TextVectorization` layer needs to be adapted as per the vocabulary from our
    # training set.
    with tf.device("/CPU:0"):
        text_vectorizer.adapt(train_dataset.map(lambda text, label: text))

    print("начали")
    train_dataset = train_dataset.map(
        lambda text, label: (text_vectorizer(text), label), num_parallel_calls=auto
    ).prefetch(auto)
    validation_dataset = validation_dataset.map(
        lambda text, label: (text_vectorizer(text), label), num_parallel_calls=auto
    ).prefetch(auto)
    test_dataset = test_dataset.map(
        lambda text, label: (text_vectorizer(text), label), num_parallel_calls=auto
    ).prefetch(auto)


    shallow_mlp_model = make_model(vocab_size_label)

    history = shallow_mlp_model.fit(
        train_dataset, validation_data=validation_dataset, epochs=epochs
    )

    _, binary_acc = shallow_mlp_model.evaluate(test_dataset)
    acc = f"{round(binary_acc * 100, 2)}%"
    print(f"Categorical accuracy on the test set: {acc}.")
    print("--- %s seconds ---" % (time.time() - start_time))

    model_for_inference = get_inference_model(shallow_mlp_model)
    # save keras inference
    save_model_params(encoder, model_for_inference, acc)
    model_for_inference.summary()

train()