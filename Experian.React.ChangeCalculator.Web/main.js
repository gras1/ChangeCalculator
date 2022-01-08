class RootComponent extends React.Component {
  constructor(props) {
      super(props);
      this.state = {
        amountOfCash: 0.00,
        cost: 0.00,
        result: ""
      };
  }

  handleSubmit = (event) => {
    event.preventDefault();
    const requestOptions = {
      method: 'POST',
      headers: { 
        'Content-Type': 'application/json; charset=UTF-8',
        'Access-Control-Allow-Origin': '*',
        'Sec-Fetch-Site': 'cross-site',
        'Referrer-Policy': 'no-referrer'
      },
      body: JSON.stringify({ 
        Currency: 'GBP',
        AmountOfCash: this.state.amountOfCash,
        Cost: this.state.cost
      })
    };
    fetch("http://localhost:5164/ChangeCalculator", requestOptions)
      .then(response => {
        if (response.ok) {
          return response.json().then((data) => {
            let totalChange = "Your change is:<br>";
            data.change.forEach(item => {totalChange += item + "<br>";});
            this.setState({result: totalChange});
          });
        } else if (response.status === 400) {
          //console.log(response);
          this.setState({result: "<div class='w3-panel w3-red'>" + response.statusText + "</div>"});
        } else {
          this.setState({result: "<div class='w3-panel w3-red'>" + response.statusText + "</div>"});
        }
      })
      .catch(err => console.error('Caught error: ', err));
  }

  render() {
    return (
<form onSubmit={this.handleSubmit}>
  <fieldset>
    <legend>Input amounts</legend>
    <div>
      <span className="gbp">
        <input name="AmountOfCash" placeholder="0.00" id="AmountOfCash" type="number" min="0.00" step="0.01" required="required" onChange={(e) => this.setState({amountOfCash:e.target.value})}></input>
        <label htmlFor="AmountOfCash">Amount Of Cash</label>
      </span>
    </div>
    <div className="clear">
      <span className="gbp">
        <input name="Cost" placeholder="0.00" id="Cost" type="number" min="0.00" step="0.01" required="required" onChange={(e) => this.setState({cost:e.target.value})}></input>
        <label htmlFor="Cost">Cost</label>
      </span>
    </div>
    <div className="clear">
      <input type="submit" className="w3-button w3-black w3-border w3-round-xlarge" value="Calculate change"></input>
    </div>
  </fieldset>
  <div id="calculatedChange" dangerouslySetInnerHTML={{__html: this.state.result}} />
</form>
    );
  }
}

ReactDOM.render(
  <RootComponent />,
  document.getElementById('root')
);