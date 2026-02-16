<p align="center">
    <img src="https://github.com/miwahn/angor/raw/patch-1/src/Angor/Client/wwwroot/angor-logo.svg" height="100" alt="Angor" />
</p>
<h3 align="center">
    Developer README
</h3>

<br>

## Table of Contents
1. [Installation](#installation)
2. [Contributing](#contributing)
3. [Running Cypress](#running-cypress)

## Installation

To run Angor locally, you need to download .NET 9 or higher. You can download it from [here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

### Steps:
1. Clone the repository:
    ```bash
    $ git clone https://github.com/block-core/angor.git
    ```

2. Navigate to the project directory:
    ```bash
    $ cd angor/src/Angor/Client
    ```

3. Build the project:
    ```bash
    $ dotnet build
    ```

4. Run the project:
    ```bash
    $ dotnet run
    ```

Now you should have Angor running locally at [http://localhost:5062/](http://localhost:5062/).

## Contributing

We welcome contributions to the codebase. Please maintain the same code style as the existing code.

### Fork workflow (for PRs)

To contribute via pull requests, use your own fork:

1. **Fork the repo** on GitHub: go to [github.com/block-core/angor](https://github.com/block-core/angor) and click "Fork".

2. **Add your fork as a remote** (replace `YOUR_USERNAME` with your GitHub username):
   ```bash
   git remote add fork https://github.com/YOUR_USERNAME/angor.git
   ```

3. **Push to your fork** instead of origin:
   ```bash
   git push fork main
   # or for a feature branch:
   git push fork your-branch-name
   ```

4. **Open a PR** on GitHub from your fork's branch to `block-core/angor` main.

## Running Cypress

To run Cypress, follow these steps:

1. Install dependencies:
    ```bash
    $ npm install
    ```

2. Ensure that Angor is running locally.

3. Open Cypress:
    ```bash
    $ npm run cypress:open
    ```
4. If you want to run the test in your terminal you can also Run cypress in headless mode:
    ```bash
    $ npm run cypress:run
    ```

