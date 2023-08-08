import firebase from 'firebase';


const config = {
    apiKey: "AIzaSyCHOkCLZMyG8ynnNBuITUzVCKQmZuMrJc8",
    authDomain: "terra-1547225268592.firebaseapp.com",
    databaseURL: "https://terra-1547225268592.firebaseio.com",
    projectId: "terra-1547225268592",
    storageBucket: "terra-1547225268592.appspot.com",
    messagingSenderId: "381445093926"
};
const fire = firebase.initializeApp(config);
export default fire;
