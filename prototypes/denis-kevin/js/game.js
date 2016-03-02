var gameProperties = {
    screenWidth: 640,
    screenHeight: 480,
};

var states = {
    game: "game",
};

// These states are represented by their index in the tank state array.
var tankStates = {
    moving: 0,
    turretRotating: 1,
    firing: 2,
    reloading: 3,
};

var tankStateStrings = ["TANK MOVEMENT", "TURRET ROTATION", "FIRE BULLETS", "RELOAD"];

// Assume that tank only has one state.
var currentTankState = tankStates.moving;
var timeUntilSwitchAllowed = 0;
var switchDelay = 500;

var tank, turret, bullets;
var timeUntilNextFire = 0, currentSpeed = 0;
var cursors, fireKey, reloadKey;
var fireRate = 500;
var bulletScale = 0.125;
var turretScale = 0.75;

var debug = false;
var bulletsLeft = 100;
var maxBullets = 100, minBullets = 0;
var currentStateDiv;
var bulletCountDiv;

var gameState = function(game) {
};

gameState.prototype = {
    preload: function() {
        currentStateDiv = document.getElementById("currentState");
        bulletCountDiv = document.getElementById("bulletCount");

        // bullet is 32 x 32
        game.load.image('bullet', 'assets/bullet.png');
        // turret is 16 x 64
        game.load.image('turret', 'assets/turret.png');
        // tank is 32 x 32
        game.load.image('tank','assets/tank.png');
    },

    create: function() {
        game.stage.backgroundColor = "#dddddd";

        game.world.setBounds(0,
            0,
            gameProperties.screenWidth,
            gameProperties.screenHeight);

        // Add tank
        tank = game.add.sprite(gameProperties.screenWidth / 2, gameProperties.screenHeight / 2, 'tank');
        tank.anchor.setTo(0.5, 0.5);
        // moving, turretRotating, firing, reloading (initially can move only)
        tank.state = [true, false, false, false];

        game.physics.enable(tank, Phaser.Physics.ARCADE);
        tank.body.drag.set(0.2);
        tank.body.maxVelocity.setTo(400, 400);
        tank.body.collideWorldBounds = true;

        turret = game.add.sprite(0, 0, 'turret');
        turret.scale.setTo(turretScale);
        // Scaled down to 8 x 32 - Want to shift it so that it's symmetrical
        // Hardcoded since I don't know how to do math.
        turret.anchor.setTo(0.5, .875);
        game.physics.enable(turret, Phaser.Physics.ARCADE);
        // turret.body.


        bullets = game.add.group();
        bullets.enableBody = true;
        bullets.scale.setTo(bulletScale, bulletScale);
        bullets.physicsBodyType = Phaser.Physics.ARCADE;
        bullets.createMultiple(bulletsLeft, 'bullet', 0, false);
        bullets.setAll('anchor.x', 0.5);
        bullets.setAll('anchor.y', 0.5);
        bullets.setAll('outOfBoundsKill', true);
        bullets.setAll('checkWorldBounds', true);

        cursors = game.input.keyboard.createCursorKeys();
        fireKey = game.input.keyboard.addKey(Phaser.KeyCode.SPACEBAR);
        reloadKey = game.input.keyboard.addKey(Phaser.KeyCode.R);
        turretRotationKeys = game.input.keyboard.addKeys({"rotateLeft": Phaser.KeyCode.Q, "rotateRight": Phaser.KeyCode.E});
        stateSwitchingKeys = game.input.keyboard.addKeys({"moving": Phaser.KeyCode.ONE, "turretRotating": Phaser.KeyCode.TWO,
                                                            "firing": Phaser.KeyCode.THREE, "reloading": Phaser.KeyCode.FOUR});

        var graphics = game.add.graphics(0, 0);
        window.graphics = graphics;
    },

    update: function() {
        // Switching tank states
        timeUntilSwitchAllowed = Math.max(0, timeUntilSwitchAllowed - game.time.elapsed);
        if (timeUntilSwitchAllowed == 0) {
            if (stateSwitchingKeys.moving.isDown && !tank.state[tankStates.moving]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.moving] = true;
                currentTankState = tankStates.moving;
                timeUntilSwitchAllowed = switchDelay;
            } else if (stateSwitchingKeys.turretRotating.isDown && !tank.state[tankStates.turretRotating]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.turretRotating] = true;
                currentTankState = tankStates.turretRotating;
                timeUntilSwitchAllowed = switchDelay;
            } else if (stateSwitchingKeys.firing.isDown && !tank.state[tankStates.firing]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.firing] = true;
                currentTankState = tankStates.firing;
                timeUntilSwitchAllowed = switchDelay;
            } else if (stateSwitchingKeys.reloading.isDown && !tank.state[tankStates.reloading]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.reloading] = true;
                currentTankState = tankStates.reloading;
                timeUntilSwitchAllowed = switchDelay;
            }
            currentStateDiv.innerHTML = tankStateStrings[currentTankState];
        }

        // Tank movement
        if (tank.state[tankStates.moving]) {
            if (cursors.left.isDown) {
                tank.angle -= 4;
            }
            else if (cursors.right.isDown) {
                tank.angle += 4;
            } else if (cursors.up.isDown) {
                currentSpeed = 150;
            } else {
                if (currentSpeed > 0) {
                    currentSpeed -= 4;
                }
            }
        }

        if (currentSpeed > 0) {
            game.physics.arcade.velocityFromRotation(tank.rotation - Math.PI / 2, currentSpeed, tank.body.velocity);
        }

        // Attach turret to tank every iteration
        turret.x = tank.x;
        turret.y = tank.y;

        // Turning the turret
        if (tank.state[tankStates.turretRotating]) {
            if (turretRotationKeys.rotateLeft.isDown) {
                turret.angle -= 4;
            } else if (turretRotationKeys.rotateRight.isDown) {
                turret.angle += 4;
            }
        }

        // Fire bullets
        timeUntilNextFire = Math.max(0, timeUntilNextFire - game.time.elapsed);
        if (tank.state[tankStates.firing]) {
            if (fireKey.isDown)
            {
                fire();
            }
        }

        if (tank.state[tankStates.reloading]) {
            if (reloadKey.isDown) {
                reload();
            }
        }

        bulletCountDiv.innerHTML = bulletsLeft;

        // Draw fire and switch reloading bars.
        var barWidth = 200, barHeight = 20;
        var startingX = gameProperties.screenWidth / 2 - barWidth / 2, startingY = 10;
        var barSeparation = 5;
        var graphics = window.graphics;
        graphics.clear();
        graphics.lineStyle(0);
        graphics.beginFill(0x005FF0, 1);
        graphics.drawRect(startingX, startingY,
            barWidth * (switchDelay - timeUntilSwitchAllowed) / switchDelay,
            barHeight);
        graphics.endFill();
        graphics.beginFill(0x00F05F, 1);
        graphics.drawRect(startingX, startingY + barHeight + barSeparation,
            barWidth * (fireRate - timeUntilNextFire) / fireRate,
            barHeight);
        graphics.endFill();
    },

    render: function() {
        if (debug) {
            // game.debug.body(tank);
            game.debug.body(turret);
            var graphics = game.add.graphics(0, 0);
            graphics.lineStyle(0);
            graphics.beginFill(0xFFFF0B, 0.5);
            graphics.drawCircle(gameProperties.screenWidth / 2, gameProperties.screenHeight / 2, 2);
            graphics.endFill();
        }
    },

};

var fire = function() {
    if (timeUntilNextFire == 0 && bulletsLeft > minBullets) {
        bulletsLeft--;
        if (bulletsLeft < minBullets) {
            bulletsLeft = minBullets;
        }

        nextFire = game.time.now + fireRate;

        var bullet = bullets.getFirstExists(false);
        var distanceOffset = 5;
        var offsetX = Math.cos(turret.rotation - Math.PI / 2) * (turret.height + distanceOffset);
        var offsetY = Math.sin(turret.rotation - Math.PI / 2) * (turret.height + distanceOffset);
        bullet.reset(1 / bulletScale * (tank.x + offsetX), 1 / bulletScale * (tank.y + offsetY));
        game.physics.arcade.velocityFromRotation(turret.rotation - Math.PI / 2, 400, bullet.body.velocity);

        timeUntilNextFire = fireRate;
    }
}

var reload = function() {
    bulletsLeft += 20;
    if (bulletsLeft > maxBullets) {
        bulletsLeft = maxBullets;
    }
};

var game = new Phaser.Game(gameProperties.screenWidth, gameProperties.screenHeight, Phaser.AUTO, 'gameDiv');
game.state.add(states.game, gameState);
game.state.start(states.game);
