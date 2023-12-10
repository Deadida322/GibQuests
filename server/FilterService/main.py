from flask import Flask, request, jsonify, make_response
from flask_cors import CORS
app = Flask(__name__)

@app.route('/')
def index():
    return 'Hello, Flask!'

@app.route('/api/check', methods=['POST'])
def check():
    json = request.json
    print(json)
    return jsonify(1)

if __name__ == '__main__':
    app.run(host='localhost', port=9008, debug=True)
    CORS(app)