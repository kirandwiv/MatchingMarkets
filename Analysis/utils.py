import numpy as np
import math
import pandas as pd
import json

def load_json(file_path):
    with open(file_path, 'r') as f:
        return json.load(f)


