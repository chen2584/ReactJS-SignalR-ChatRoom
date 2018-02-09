import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import { HubConnection } from '@aspnet/signalr-client';

class App extends Component {

  constructor() {
    super();
    this.state = { name: "", disableInput: true };
  }
  
  componentDidMount(){
    this.onConnect();

  }

  hubConnection;
  messageInfo = [];
  inputRef;
  onConnect() {
    let name = null;
    while(name === null || name === "") {
      name = prompt("Input your display name");
    }

    //this.hubConnection = new HubConnection(`http://localhost:5000/chatserver?userName=${name}`);
    this.hubConnection = new HubConnection(`/chatserver?userName=${name}`);
    this.hubConnection.start();

    this.hubConnection.onclose(() => {
      this.setState({ disableInput: false });
      alert("การเชื่อมต่อเกิดปัญหา");
    });

    //ตอนกลับจากเซฺฟเวอร์
    this.hubConnection.on("onReceiveMessage", (name, nameColor, message) => {
      this.messageInfo.push({ name: name, nameColor: nameColor, message: message});
      if(this.messageInfo.length > 40) { //ถ้าจำนวนข้อความมากกว่า 40 ให้ตัดข้อความบนสุดออก
        this.messageInfo.splice(0, 1);
      }
      this.forceUpdate();
    });

    this.setState({ disableInput: false }, () => { this.refs.inputRef.focus(); });
  }

  chatMessage = "";
  handleChange = (e) => {
    this.chatMessage = e.target.value;
  }

  onSubmitMessage = (e) => {
    e.preventDefault();
    this.hubConnection.invoke("onSendMessage", this.chatMessage);

    this.chatMessage = "";
    this.refs.inputRef.value = "";
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">ReactJS ChatRoom</h1>
        </header>
        <p className="App-intro">
          Simple ReactJS + ASP.NET Core + SignalR Alpha
        </p>
        <div className="ChatBox">
          <div className="messageBox">
            {
              this.messageInfo.map((result, index) => {
                return (
                  <div key={index}>
                    <span style={{ color: result.nameColor}}>{result.name}</span>: {result.message}
                  </div>
                );

              })
            }
          </div>
          <div className="userInput">
            <form onSubmit={this.onSubmitMessage}>
              <input type="text" className="txtInput" onChange={this.handleChange} 
                placeholder="Type message here..." disabled={this.state.disableInput}
                ref="inputRef"/>
              <input type="submit" className="btnSend" value="Send" />
            </form>
          </div>
        </div>

        {
          this.state.disableInput &&  
          (
            <div className="disableScreen">
            </div>
          )
        }
      </div>
    );
  }
}

export default App;
