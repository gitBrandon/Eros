const {
    app,
    BrowserWindow
} = require('electron')

const {
    ipcMain
} = require('electron')

let mainWindow

app.on('ready', function() {
    mainWindow = new BrowserWindow({
        width: 1024,
        height: 768
    });

    mainWindow.loadURL('file://' + __dirname + '/index.html');
    mainWindow.show();
});

app.on('window-all-closed', function() {
    if (process.platform != 'darwin')
        app.quit();
});