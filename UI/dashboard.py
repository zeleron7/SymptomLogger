import requests
import streamlit as st

pagination = {
    "pageNumber": 1,
    "pageSize": 10
}

response = requests.get(
    "https://localhost:7109/api/v1/Conditions", 
    params=pagination,
    verify=False
)

print(response.json())