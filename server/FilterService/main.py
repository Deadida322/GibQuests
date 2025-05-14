from flask import Flask, request, jsonify, make_response
from flask_cors import CORS
import pickle
from sklearn.model_selection import train_test_split

from preprocessing import lemmed_and_clear_stop_words
from reader import get_texts
from vectorizers import countVectorizer
from model_test import predict

app = Flask(__name__)

@app.route('/')
def index():
    return 'Hello, Flask!'

@app.route('/api/check', methods=['POST'])
def check():
    json = request.json
    text = json["text"]
    lemmed_words = lemmed_and_clear_stop_words(text)
    rating = predict(' '.join(lemmed_words))
    return jsonify({"rating": str(rating)})

if __name__ == '__main__':
    app.run(host='localhost', port=9008, debug=True)
    CORS(app)