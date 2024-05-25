from flask import Flask, request, jsonify, make_response
from flask_cors import CORS
import pickle
from sklearn.model_selection import train_test_split
from reader import get_texts
from utils import vectorizer

app = Flask(__name__)

@app.route('/')
def index():
    return 'Hello, Flask!'

@app.route('/api/check', methods=['POST'])
def check():
    json = request.json
    with open('model.pkl', 'rb') as f:
        classifier = pickle.load(f)
        textsWithCategory = get_texts(3000)
        texts = [text for text, category in textsWithCategory]
        labels = [category for text, category in textsWithCategory]
        X_train, X_test, y_train, y_test = train_test_split(texts, labels, test_size=0.1, random_state=42)

        X_train_vectorized = vectorizer.fit_transform(X_train)
        classifier.fit(X_train_vectorized, y_train)

        X_test_vectorized = vectorizer.transform([json['text']])
        predict = classifier.predict(X_test_vectorized)
    return jsonify({"rating": str(predict[0])})

if __name__ == '__main__':
    app.run(host='localhost', port=9008, debug=True)
    CORS(app)