from sklearn.datasets import fetch_20newsgroups
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from tensorflow.keras.layers import Dense, Dropout
from keras.src.callbacks import EarlyStopping
import tensorflow as tf
# Load the 20 newsgroups dataset
newsgroups_train = fetch_20newsgroups(subset='train', categories=['alt.atheism', 'sci.space'])
newsgroups_test = fetch_20newsgroups(subset='test', categories=['alt.atheism', 'sci.space'])

X_train = newsgroups_train.data
y_train = newsgroups_train.target
X_test = newsgroups_test.data
y_test = newsgroups_test.target

vectorizer = TfidfVectorizer(max_features=5000)

# Fit and transform the training data
X_train_tfidf = vectorizer.fit_transform(X_train)
X_test_tfidf = vectorizer.transform(X_test)

X_train_tfidf, X_val_tfidf, y_train, y_val = train_test_split(X_train_tfidf, y_train, test_size=0.1, random_state=42)

# Define the neural network model
model = tf.keras.Sequential([
    Dense(128, input_shape=(X_train_tfidf.shape[1],), activation='relu'),
    Dropout(0.5),
    Dense(64, activation='relu'),
    Dropout(0.5),
    Dense(1, activation='sigmoid')
])

# Compile the model
model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])

early_stopping = EarlyStopping(monitor='val_loss', patience=3, verbose=1)

# Convert sparse matrices to dense matrices
X_train_tfidf = X_train_tfidf.toarray()
X_val_tfidf = X_val_tfidf.toarray()
X_test_tfidf = X_test_tfidf.toarray()

# Train the model
print(y_test)
# history = model.fit(X_train_tfidf, y_train, epochs=10, batch_size=32, validation_data=(X_val_tfidf, y_val), callbacks=[early_stopping])