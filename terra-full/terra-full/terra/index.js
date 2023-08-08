import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import 'bootstrap/dist/css/bootstrap.min.css';
//Router v4
import {BrowserRouter} from "react-router-dom";

ReactDOM.render(
 <BrowserRouter><App /></BrowserRouter>,
 document.getElementById('root'));
registerServiceWorker();
