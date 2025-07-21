import React from "react";
import ReactDOM from "react-dom/client";

// javascript
// const heading = document.createElement("h1");

// heading.innerHTML = "Hello World from Javascript!";
// var root = document.getElementById("root");
// root.appendChild(heading);

const heading = React.createElement("h1", { id:"heading" }, "Namaste React");

console.log(heading);

const jsxHeading = <h1 className="head">Namaste React using JSX !</h1>

console.log(jsxHeading);

const root = ReactDOM.createRoot(document.getElementById("root"));

const element = <span>Span React Element</span>;

const Title = () => (
    <h1 className="head">Namaste React using JSX !</h1>
);

const titleElement = (
    <h1 className="head">
        {element}<br></br><br></br>
        Namaste React using JSX Element !
    </h1>
)

//React Functional Component
const HeadingComponent = () => (
    <div id="container">
        <Title/>
        {titleElement}
        <h1 className="heading">Namaste React from React Functional Component</h1>
    </div>
);

root.render(<HeadingComponent/>);


