"use strict";

import React from 'react';
import ReactDOM from 'react-dom';
import HelloWorld from './components/helloworld';

ReactDOM.render(
    <HelloWorld message ="Hello World From React" />,
    document.getElementById('app')
);
