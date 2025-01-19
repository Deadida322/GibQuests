from flask import Flask, request, jsonify, make_response
from flask_cors import CORS
import pickle
from sklearn.model_selection import train_test_split
from reader import get_texts
from vectorizers import countVectorizer
from netural import get_rating
app = Flask(__name__)

@app.route('/')
def index():
    return 'Hello, Flask!'

@app.route('/api/check', methods=['POST'])
def check():
    json = request.json
    return jsonify({"rating": str(16)})

if __name__ == '__main__':
    app.run(host='localhost', port=9008, debug=True)
    CORS(app)