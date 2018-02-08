var tabNames = ["Accounts", "Categories", "Payees", "Reports", "Transactions"];

//Tabs
class ActiveTab extends React.Component {
    render() {
        return <div className="tabbed-tab tabbed-activetab" onClick={() => { this.props.clickHandler(this.props.tabName) }}>{this.props.tabName}</div>;
    }
};

class Tab extends React.Component {
    render() {
        return <div className="tabbed-tab" onClick={() => { this.props.clickHandler(this.props.tabName) }}>{this.props.tabName}</div>;
    }
};

class Tabs extends React.Component{ 
    render() {
        const st = this.props.selectedTab;
        const ch = this.props.clickHandler;
        return (
            this.props.tabNames.map(function (tabName) {
                if (tabName == st) {
                    return <ActiveTab tabName={tabName} clickHandler={ch}/>;
                }
                else {
                    return <Tab tabName={tabName} clickHandler={ch}/>;
                }
            })           
        );
    }
};

//Accounts
class Account extends React.Component{
    render() {
        return (
            <div className="adminItem">
                <div className="adminField">{this.props.id}</div>
                <div className="adminField">{this.props.type}</div>
                <div className="adminField">{this.props.name}</div>
                <div className="adminField">{this.props.number}</div>
            </div>
        );
    }
};

class Accounts extends React.Component{
    getData() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', '../Accounts/GetAccounts', false);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.props.data = data;
        }.bind(this);
        xhr.send();
    };

    render() {
        this.getData();
        var accountNodes = this.props.data.map(function (account) {
            return (
                <Account id={account.Id} type={account.Type} name={account.Name} number={account.Number} />
            );
        });

        return (
            <div className="adminGroup">
                <h3>Accounts</h3>
                <div className="adminTable">
                    <div className="adminHeader">
                        <div className="adminField">Id</div>
                        <div className="adminField">Type</div>
                        <div className="adminField">Name</div>
                        <div className="adminField">Number</div>
                    </div>
                    <div class="adminBody">
                        {accountNodes}
                    </div>
                </div>
            </div>
        );
    }
};

//Categories
class Category extends React.Component{

render() {
    if (this.props.tax) {
        taxSpan = <span>&#10004;</span>;
    }
    else {
        taxSpan = <span>&nbsp;</span>;
    }

    return (
            <div className="adminItem">
                <div className="adminField">{this.props.id}</div>
                <div className="adminField">{this.props.name}</div>
                <div className="adminField">
                    {taxSpan}
                </div>
                <div className="adminField">{this.props.type}</div>
            </div>
        );
    }
};

class Categories extends React.Component{
    getData() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', '../Categories/GetCategories', false);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.props.data = data;
        }.bind(this);
        xhr.send();
    };

    render() {
        this.getData();
        var otherCategoryNodes = this.props.data.OtherCategories.map(function (category) {
            return (
                <Category id={category.Id} name={category.Name} tax={category.Tax} type={category.Type}  />
            );
        });

        var incomeCategoryNodes = this.props.data.IncomeCategories.map(function (category) {
            return (
                <Category id={category.Id} name={category.Name} tax={category.Tax} type={category.Type} />
            );
        });

        var expenseCategoryNodes = this.props.data.ExpenseCategories.map(function (category) {
            return (
                <Category id={category.Id} name={category.Name} tax={category.Tax} type={category.Type} />
            );
        });

        var categoryDivider = function () {
            return (
                <div className="adminItem">
                    <div className="adminDivider"></div>
                    <div className="adminDivider"></div>
                    <div className="adminDivider"></div>
                    <div className="adminDivider"></div>
                </div>
            );
        };


        return (
            <div className="adminGroup">
                <h3>Categories</h3>
                <div className="adminTable">
                    <div className="adminHeader">
                        <div className="adminField">Id</div>
                        <div className="adminField">Name</div>
                        <div className="adminField">Tax</div>
                        <div className="adminField">Type</div>
                    </div>
                    <div className="adminBody">
                        {otherCategoryNodes}
                        {categoryDivider}
                        {incomeCategoryNodes}
                        {categoryDivider}
                        {expenseCategoryNodes}
                </div>
            </div>
        </div>
        );
    }
};


//Payees
class Payee extends React.Component{
    render() {
        return (
            <div className="adminItem">
                <div className="adminField">{this.props.id}</div>
                <div className="adminField">{this.props.name}</div>
            </div>
        );
    }
};

class Payees extends React.Component{
    getData() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', '../Payees/GetPayees', false);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.props.data = data;
        }.bind(this);
        xhr.send();
    };

    render() {
        this.getData();
        var payeeNodes = this.props.data.map(function (payee) {
            return (
                <Payee id={payee.Id} name={payee.Name} />
            );
        });

        return (
            <div className="adminGroup">
                <h3>Payees</h3>
                <div className="adminTable">
                    <div className="adminHeader">
                        <div className="adminField">Id</div>
                        <div className="adminField">Name</div>
                    </div>
                    <div className="adminBody">
                        {payeeNodes}
                    </div>
                </div>
            </div>
        );
    }
};

//Reports
class Report extends React.Component{
    render() {
        if (this.props.taxOnly) {
            var taxSpan = <span>&#10004;</span>;
        }
        else {
            var taxSpan = <span>&nbsp;</span>;
        }
        return (
            <div className="adminItem">
                <div className="adminField">{this.props.id}</div>
                <div className="adminField">{this.props.name}</div>
                <div className="adminField">{this.props.type}</div>
                <div className="adminField">
                    {taxSpan}
                </div>
            </div>
        );
    }
};

class Reports extends React.Component{
    getData() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', '../Reports/GetReports', false);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.props.data = data;
        }.bind(this);
        xhr.send();
    };

    render() {
        this.getData();
        var reportNodes = this.props.data.map(function (report) {
            return (
                <Report id={report.Id} name={report.Name} taxOnly={report.TaxOnly} type={report.Type} />
            );
        });

        return (
            <div className="adminGroup">
                <h3>Reports</h3>
                <div className="adminTable">
                    <div className="adminHeader">
                        <div className="adminField">Id</div>
                        <div className="adminField">Name</div>
                        <div className="adminField">Type</div>
                        <div className="adminField">TaxOnly</div>
                    </div>
                    <div className="adminBody">
                        {reportNodes}
                    </div>
                </div>
            </div>
        );
    }
};

//Transactions
class Transaction extends React.Component{
    render() {
        //var ondate = new Date(this.props.onDate);
        ondate = new Date(parseInt(this.props.onDate.replace("/Date(", "").replace(")/", ""), 10));
        return (
            <div class="adminItem">
                <div class="adminField">{this.props.id}</div>
                <div class="adminField">{ondate.toDateString()}</div>
                <div class="adminField">{this.props.payee}</div>
                <div class="adminField">{this.props.fromAccount}</div>
                <div class="adminField">{this.props.toAccount}</div>
                <div class="adminField">${this.props.amount}</div>
            </div>
        );
    }
};

class Transactions extends React.Component{
    getData() {
        var xhr = new XMLHttpRequest();
        xhr.open('get', '../Transactions/GetTransactions', false);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.props.data = data;
        }.bind(this);
        xhr.send();
    };

    render() {
        this.getData();
        var transactionNodes = this.props.data.map(function (transaction) {
            return (
                <Transaction id={transaction.Id} amount={transaction.Amount} onDate={transaction.OnDate} fromAccount={transaction.FromAccount} payee={transaction.Payee} tax={transaction.Tax} toAccount={transaction.ToAccount} />
            );
        });

        return (
            <div class="adminGroup">
                <h3>Transactions</h3>
                <div class="adminTable">
                    <div class="adminHeader">
                        <div class="adminField">Id</div>
                        <div class="adminField">Date</div>
                        <div class="adminField">Payee</div>
                        <div class="adminField">From</div>
                        <div class="adminField">To</div>
                        <div class="adminField">Amount</div>
                    </div>
                    <div class="adminBody">
                        {transactionNodes}
                    </div>
                </div>
            </div>
        );
    }
};

//Content
class AdminContent extends React.Component{

    constructor(props) {
        super(props);
        this.state = { selectedTab: 'Accounts' };
    };

    selectTab(tabName) {
        console.log('!! ' + tabName + ' tab was selected !!');
        this.setState({ selectedTab: tabName });
    }

    content(selectedTab) {
        switch (selectedTab) {
            case 'Accounts':
                return <Accounts />;
            case 'Categories':
                return <Categories />;
            case 'Payees':
                return <Payees />;
            case 'Reports':
                return <Reports />;
            case 'Transactions':
                return <Transactions />;
            default:
                return null;
        }
    };
 
    render() {
        console.log('AdminContent rendering with selectedTab: ' + (this.state.selectedTab ? this.state.selectedTab : 'null'));
        return (
            <div>
                <div className="tabbed-tabs">
                    <Tabs tabNames={tabNames} selectedTab={this.state.selectedTab} clickHandler={this.selectTab.bind(this)} />
                </div>
                <div className="tabbed-content">
                    {this.content(this.state.selectedTab)}
                </div>
            </div>
        );
    }
};


ReactDOM.render(
    <AdminContent />,
    document.getElementById('content')
);




//<Tabs tabNames={tabNames} selected={this.state.selected} clickHandler={this.selectTab} />
//{ this.tabs(this.tabNames, this.state.selectedTab) }