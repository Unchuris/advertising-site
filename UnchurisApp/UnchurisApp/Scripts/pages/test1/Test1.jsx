import React from 'react';
var SelectFieldUI = require('material-ui/SelectField');

class Test1 extends React.Component {
    static propTypes = {
        text: React.PropTypes.string
    };

  render() {
      return (
          <h1>Hello, {this.props.text}</h1>
      );
  }
}
module.exports = Test1;