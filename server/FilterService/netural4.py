import numpy as np

from tensorflow.keras import Sequential, Model
from tensorflow.keras.layers import Dense, LSTM, Embedding,Dropout,SpatialDropout1D,Conv1D,MaxPooling1D,GRU,BatchNormalization
from tensorflow.keras.layers import Input,Bidirectional,GlobalAveragePooling1D,GlobalMaxPooling1D,concatenate,LeakyReLU
from tensorflow.keras import regularizers
from tensorflow.keras import backend as K
from sklearn.model_selection import train_test_split
from tensorflow.keras.utils import to_categorical
from keras_preprocessing.text import Tokenizer



from reader import get_texts_3


textsWithCategory = get_texts_3(0, 100)
print('Получили тексты')
texts = [text for text, category in textsWithCategory]
labels = [category for text, category in textsWithCategory]

tokenizer = Tokenizer(num_words=100000)
tokenizer.fit_on_texts(texts)
word_index = tokenizer.word_index

text_embedding = np.zeros((len(word_index) + 1, 300))
for word, i in word_index.items():
   text_embedding[i] = nlp(word).vector


model = Sequential()
model.add(
Embedding(input_dim=text_embedding.shape[0],
output_dim=text_embedding.shape[1],
weights=[text_embedding],
input_length=MAX_SEQUENCE_LENGTH,
trainable=False))
model.add(SpatialDropout1D(0.5))
model.add(Conv1D(filters, kernel_size=kernel_size,kernel_regularizer=regularizers.l2(0.00001), padding='same'))
model.add(LeakyReLU(alpha=0.2))
model.add(MaxPooling1D(pool_size=2))
model.add(Bidirectional(LSTM(lstm_units,dropout=0.5, recurrent_dropout=0.5,return_sequences=True)))
model.add(SpatialDropout1D(0.5))
model.add(Conv1D(filters, kernel_size=kernel_size,kernel_regularizer=regularizers.l2(0.00001), padding='same'))
model.add(LeakyReLU(alpha=0.2))
model.add(MaxPooling1D(pool_size=2))
model.add(Bidirectional(LSTM(lstm_units,dropout=0.5, recurrent_dropout=0.5,return_sequences=True)))
model.add(SpatialDropout1D(0.5))
model.add(Conv1D(filters, kernel_size=kernel_size,kernel_regularizer=regularizers.l2(0.00001), padding='same'))
model.add(LeakyReLU(alpha=0.2))
model.add(MaxPooling1D(pool_size=2))
model.add(Bidirectional(LSTM(lstm_units,dropout=0.5, recurrent_dropout=0.5)))
model.add(Dense(4,activation='softmax'))
model.compile(optimizer='adam',loss='categorical_crossentropy', metrics=['accuracy'])

categorical_labels = to_categorical(labels,num_classes=4)
X_train, X_test, Y_train, Y_test = train_test_split(texts, categorical_labels, test_size=0.2)

model.fit(pad_sequences(tokenizer.texts_to_sequences(X_train),maxlen=MAX_SEQUENCE_LENGTH),Y_train,batch_size=512,epochs=10,
validation_data=(pad_sequences(tokenizer.texts_to_sequences(X_test),maxlen=MAX_SEQUENCE_LENGTH),Y_test),callbacks=callbacks_list,shuffle=True)

result = model.predict_on_batch(pad_sequences(tokenizer.texts_to_sequences([
" What happened 2 ur vegan food options?! At least say on ur site so i know I won't be able 2 eat anything for next 6 hrs #fail",
" I am really scared of the future",
"everything is great, I am doing awesome"])
,maxlen=MAX_SEQUENCE_LENGTH))
print("result: ", np.argmax(result,axis=-1),"\n")